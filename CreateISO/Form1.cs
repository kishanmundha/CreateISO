using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

//using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using TL.IsoImage;

namespace CreateISO
{
    public partial class Form1 : Form
    {
        private IsoFromMedia iso;
        private IsoState status;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            RefreshDriveList();
        }

        private void RefreshDriveList()
        {
            DriveList.Items.Clear();

            DriveInfo[] dc = DriveInfo.GetDrives();

            foreach (DriveInfo d in dc)
            {
                if (d.DriveType == DriveType.CDRom)
                {
                    DriveList.Items.Add(d.RootDirectory);
                }
            }

            if (DriveList.Items.Count > 0)
            {
                DriveList.SelectedIndex = 0;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                SavePath.Text = saveFileDialog1.FileName;
            }
        }

        private void Refresh_Drive_Click(object sender, EventArgs e)
        {
            RefreshDriveList();
        }

        private void Start_Click(object sender, EventArgs e)
        {
            iso = new IsoFromMedia();
            status = iso.CreateIsoFromMedia(DriveList.Text, SavePath.Text);
            if (status == IsoState.Running)
            {
                statusPanel.Visible = true;
                errPanel.Visible = false;
                errByteLabel.Visible = false;
                timer1.Start();
            }
            else
            {
                iso.Stop();
                switch (status)
                {
                    case IsoState.NotReady:
                        errMsg.Text = "Error: Device not ready";
                        break;
                    case IsoState.NotEnoughMemory:
                        errMsg.Text = "Error: Not Enough Memory on Disk";
                        break;
                    case IsoState.InvalidHandle:
                        errMsg.Text = "Error: Not Enough Memory on Disk";
                        break;
                    case IsoState.LimitExceeded:
                        errMsg.Text = "Error: Limit Exceeded";
                        break;
                    case IsoState.NoDevice:
                        errMsg.Text = "Error: No Device";
                        break;
                    default:
                        errMsg.Text = "Error: Unknown error";
                        break;
                }
                errPanel.Visible = true;
                statusPanel.Visible = false;
            }
        }

        private void Abort_Click(object sender, EventArgs e)
        {
            if (status == IsoState.Running)
            {
                iso.Stop();
                MessageBox.Show("Aborted");
            }
            timer1.Stop();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            progressBar.Value = iso.ProgressPrecent;
            long KB = iso.PassByte / 1024;
            long MB = KB / 1024;
            long total_KB = iso.MediumSize / 1024;
            long total_MB = total_KB / 1024;
            statusMsg.Text = MB + " MB (" + KB + " KB) / " + total_MB + " MB (" + total_KB + " KB)";

            if (iso.errByte > 0)
            {
                KB = iso.errByte / 1024;
                MB = KB / 1024;
                errByteLabel.Text = MB + " MB (" + KB + " KB)";
                errByteLabel.Visible = true;
            }

            if (iso.errMsg != null)
            {
                errMsg.Text = iso.errMsg;
                errPanel.Visible = true;
            }
            else
            {
                errPanel.Visible = false;
            }
        }
    }
}

namespace TL.IsoImage
{
    /// <summary>
    /// Allows to create ISO images from media (CD / DVD)
    /// </summary>
    public class IsoFromMedia
    {
        #region Variables
        /// <summary>
        /// BackgroundWorker for creating the ISO file
        /// </summary>
        BackgroundWorker bgCreator;

        /// <summary>
        /// FileStream for reading
        /// </summary>
        FileStream streamReader;

        /// <summary>
        /// FileStream for writing
        /// </summary>
        FileStream streamWriter;
        #endregion

        #region Constants
        /// <summary>
        /// 128 KB block size
        /// </summary>
        const int BUFFER = 0x20000;

        /// <summary>
        /// 4 GB maximum size per file on FAT32 file system
        /// </summary>
        const long LIMIT = 4294967296;
        #endregion

        /// <summary>
        /// Get Progress Size
        /// </summary>
        public int ProgressPrecent { get; private set; }

        /// <summary>
        /// Get Pass Byte
        /// </summary>
        public long PassByte { get; private set; }

        /// <summary>
        /// Get Error Message
        /// </summary>
        public string errMsg { get; private set; }

        /// <summary>
        /// Error Data
        /// </summary>
        public long errByte { get; private set; }

        /// <summary>
        /// Current status
        /// </summary>
        public IsoState status { get; private set; }

        /// <summary>
        /// Error Counter
        /// </summary>
        private int errCount { get; set; }

        #region Properties
        /// <summary>
        /// Path to the ISO file
        /// </summary>
        string PathToIso { get; set; }

        /// <summary>
        /// Size of the medium
        /// </summary>
        public long MediumSize { get; set; }

        /// <summary>
        /// Medium handle
        /// </summary>
        SafeFileHandle Handle { get; set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public IsoFromMedia()
        {
            bgCreator = new BackgroundWorker();
            bgCreator.WorkerSupportsCancellation = true;
            bgCreator.DoWork += new DoWorkEventHandler(bgCreator_DoWork);
            bgCreator.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgCreator_RunWorkerCompleted);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Starts the thread creating the ISO file
        /// </summary>
        void bgCreator_DoWork(object sender, DoWorkEventArgs e)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            try
            {
                streamReader = new FileStream(Handle, FileAccess.Read, BUFFER);
                streamWriter = new FileStream(PathToIso, FileMode.Create, FileAccess.Write, FileShare.None, BUFFER);

                byte[] buffer = new byte[BUFFER];
                errByte = 0;

                //Read buffer blocks from source and write them to the ISO file
                do
                {
                    if (bgCreator.CancellationPending)
                    {
                        e.Cancel = true;
                        Stop();
                        errMsg = "Creation of the images was canceled";
                        break;
                    }

                    try
                    {
                        if (errCount > 8)
                        {
                            streamReader.Position -= BUFFER;
                            bool test = false;

                            int testValue = 8;
                            bool found = false;
                            do
                            {
                                if (!test) streamReader.Position += (BUFFER * testValue);
                                else streamReader.Position -= ((BUFFER * testValue) - BUFFER);
                                try
                                {
                                    streamReader.Read(buffer, 0, BUFFER);
                                    test = true;
                                    if (testValue == 1)
                                    {
                                        streamReader.Position -= BUFFER;
                                        found = true;
                                    }
                                    if (testValue != 1)
                                    {
                                        testValue /= 2;
                                    }
                                }
                                catch (Exception exxx)
                                {
                                    test = false;
                                    if (testValue != 8 && testValue != 1)
                                    {
                                        testValue /= 2;
                                    }
                                    if (testValue == 1)
                                    {
                                        streamReader.Position += BUFFER;
                                        found = true;
                                    }
                                }
                                PassByte = streamReader.Position;
                                ProgressPrecent = Convert.ToInt32((streamReader.Position * 100) / MediumSize);
                            } while (!found);
                            errByte += (streamReader.Position - streamWriter.Length);
                            buffer = new byte[BUFFER];
                            while (streamWriter.Length < streamReader.Position)
                            {
                                streamWriter.Write(buffer, 0, BUFFER);
                            }
                        }
                        streamReader.Read(buffer, 0, BUFFER);
                        errMsg = null;
                        errCount = 0;
                    }
                    catch (Exception exx)
                    {
                        errMsg = "Error: " + exx.Message;
                        buffer = new byte[BUFFER];
                        streamReader.Position += BUFFER;
                        errByte += BUFFER;
                        errCount++;
                    }
                    streamWriter.Write(buffer, 0, BUFFER);

                    //Progress in percent
                    int percent = Convert.ToInt32((streamWriter.Length * 100) / MediumSize);

                    ProgressPrecent = percent;
                    PassByte = streamWriter.Length;
                } while (streamReader.Position == streamWriter.Position);
            }
            catch (Exception ex)
            {
                errMsg = "Error: " + ex.Message;
            }
            finally
            {
                /*if (OnFinish != null)
                {
                    EventIsoArgs eArgs = new EventIsoArgs(stopWatch.Elapsed);
                    OnFinish(eArgs);
                }*/
                if (status != IsoState.Aborted)
                    MessageBox.Show("Image Created Successfully");
            }
        }

        /// <summary>
        /// When the file is finished
        /// </summary>
        void bgCreator_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            CloseAll();
        }

        /// <summary>
        /// Creates an ISO image from media (CD/DVD)
        /// </summary>
        /// <param name="source">CD/DVD</param>
        /// <param name="destination">Path where the ISO file is to be stored</param>
        /// <returns>
        /// Running = Creation in progress
        /// InvalidHandle = Invalid handle
        /// NoDevice = The source is not a medium (CD/DVD)
        /// NotEnoughMemory = Not enough disk space
        /// LimitExceeded = Source exceeds FAT32 maximum file size of 4 GB (4096 MB)
        /// NotReady = The device is not ready
        /// </returns>
        public IsoState CreateIsoFromMedia(string source, string destination)
        {
            //Is the device ready?
            if (!new DriveInfo(source).IsReady)
                return IsoState.NotReady;

            //Source CD/DVD
            if (new DriveInfo(source).DriveType != DriveType.CDRom)
                return IsoState.NoDevice;

            //Get medium size
            MediumSize = GetMediumLength(source);

            //Check disk space
            long diskSize = new DriveInfo(Path.GetPathRoot(destination)).AvailableFreeSpace;

            if (diskSize <= MediumSize)
                return IsoState.NotEnoughMemory;

            //Check capacity of > 4096 MB (NTFS)
            if (!CheckNTFS(destination) && MediumSize >= LIMIT)
                return IsoState.LimitExceeded;

            //Create handle
            Handle = Win32.CreateFile(source);

            if (!string.IsNullOrEmpty(destination))
                PathToIso = destination;

            //If invalid or closed handle
            if (Handle.IsInvalid || Handle.IsClosed)
                return IsoState.InvalidHandle;

            //Create thread to create the ISO file
            bgCreator.RunWorkerAsync();

            return IsoState.Running;
        }

        /// <summary>
        /// Aborts the creation of the image and deletes it
        /// </summary>
        public void Stop()
        {
            CloseAll();

            if (File.Exists(PathToIso))
                File.Delete(PathToIso);

            status = IsoState.Aborted;
        }

        /// <summary>
        /// Closes all streams and handles and frees resources
        /// </summary>
        private void CloseAll()
        {
            if (bgCreator != null)
            {
                bgCreator.CancelAsync();
                bgCreator.Dispose();
            }

            if (streamReader != null)
            {
                streamReader.Close();
                streamReader.Dispose();
            }

            if (streamWriter != null)
            {
                streamWriter.Close();
                streamWriter.Dispose();
            }

            if (Handle != null)
            {
                Handle.Close();
                Handle.Dispose();
            }
        }

        /// <summary>
        /// Size of media (CD/DVD)
        /// </summary>
        /// <param name="drive">Source drive</param>
        /// <returns>Size in bytes</returns>
        private long GetMediumLength(string drive)
        {
            return new DriveInfo(drive).TotalSize;
        }

        /// <summary>
        /// Checks if filesystem is NTFS
        /// </summary>
        /// <param name="destination">Path to ISO file</param>
        /// <returns>True if NTFS</returns>
        private bool CheckNTFS(string destination)
        {
            return new DriveInfo(Path.GetPathRoot(destination)).DriveFormat == "NTFS" ? true : false;
        }
        #endregion
    }

    #region Enumeration
    /// <summary>
    /// Returns state of ISO creation
    /// </summary>
    public enum IsoState
    {
        /// <summary>
        /// Creation running
        /// </summary>
        Running = 1,
        /// <summary>
        /// Invalid handle
        /// </summary>
        InvalidHandle = -1,
        /// <summary>
        /// The source is no CD/DVD media
        /// </summary>
        NoDevice = -2,
        /// <summary>
        /// Not enough memory remaining
        /// </summary>
        NotEnoughMemory = -3,
        /// <summary>
        /// Source exceeds FAT32 maximum file size of 4 GB (4096 MB)
        /// </summary>
        LimitExceeded = -4,
        /// <summary>
        /// The device is not ready
        /// </summary>
        NotReady = -5,
        /// <summary>
        /// Aborted
        /// </summary>
        Aborted = -6
    }
    #endregion

    #region Win32
    /// <summary>
    /// Provices required functionality
    /// </summary>
    internal class Win32
    {
        /// <summary>
        /// Read access
        /// </summary>
        static uint GENERIC_READ = 0x80000000;

        /// <summary>
        /// Indicates that subsequent opening operations are successful only when read access to the object is requested
        /// </summary>
        static uint FILE_SHARE_READ = 0x1;

        /// <summary>
        /// Opens the file. Fails if file not exists
        /// </summary>
        static uint OPEN_EXISTING = 0x3;

        /// <summary>
        /// Indicates that the file has no other attributes. This attribute is valid only if it is used alone.
        /// </summary>
        static uint FILE_ATTRIBUTE_NORMAL = 0x00000080;

        /// <summary>
        /// Returns handle that can be used to access a file or device in different ways
        /// </summary>
        /// <param name="lpFileName">Name of file or device to be opened</param>
        /// <param name="dwDesiredAccess">Access to requested file or device</param>
        /// <param name="dwShareMode">Requested share mode of file or device</param>
        /// <param name="lpSecurityAttributes">Pointer to a security attribute</param>
        /// <param name="dwCreationDisposition">An action, which is performed on a file or a device if it is present or not present</param>
        /// <param name="dwFlagsAndAttributes">File/device attribute, FILE_ATTRIBUTE_NORMAL is most frequently used</param>
        /// <param name="hTemplateFile">Handle to a template file</param>
        /// <returns>A handle to the device/file</returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr CreateFile(string lpFileName, uint dwDesiredAccess, uint dwShareMode, IntPtr lpSecurityAttributes, uint dwCreationDisposition, uint dwFlagsAndAttributes, IntPtr hTemplateFile);

        /// <summary>
        /// Creates the handle of the media
        /// </summary>
        /// <param name="device">Media (CD/DVD)</param>
        /// <returns>Handle of media</returns>
        public static SafeFileHandle CreateFile(string device)
        {
            //Check how drive letter was entered
            //e.g. Z:\ -> Z: else change nothing
            string devName = device.EndsWith(@"\") ? device.Substring(0, device.Length - 1) : device;

            //Create handle
            IntPtr devHandle = CreateFile(string.Format(@"\\.\{0}", devName), GENERIC_READ, FILE_SHARE_READ, IntPtr.Zero,
                OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, IntPtr.Zero);

            return new SafeFileHandle(devHandle, true);
        }
    }
    #endregion
}


