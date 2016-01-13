using System;
using System.Collections.Generic;
using TeamViewerPopupBlocker.Forms;
using TeamViewerPopupBlocker.Properties;

namespace TeamViewerPopupBlocker.Classes.Notifications
{
    public sealed class ProgramStatus
    {
        private static ProgramStatus instance = null;

        private static readonly object syncRoot = new Object();

        public static ProgramStatus Instance
        {
            get
            {
                lock (syncRoot)
                {
                    return instance ?? (instance = new ProgramStatus());
                }
            }
        }

        private List<Status> statusList = new List<Status>();

        private DateTime tTimeStatusReady = DateTime.Now;

        public void AddStatus(Status srcStatus)
        {
            statusList.Add(srcStatus);
        }

        public void GetStatus(MainForm mainForm)
        {
            if (statusList.Count <= 0) return;

            bool tContnue = DateTime.Now > tTimeStatusReady;

            if (!tContnue) return;

            Status status = statusList[0].Clone();

            if (status == null)
            {
                return;
            }

            if (status.TimeOutMinimum > 0)
            {
                this.tTimeStatusReady = DateTime.Now.AddMilliseconds(status.TimeOutMinimum);
            }

            switch (status.StatusType)
            {
                case StatusType.StartBlocking:
                {
                    mainForm.ShowNotificationMessage(status.StatusType,
                        Resources.ProgramStatus_GetStatus_Blocking_started, status.TimeOut);
                    break;
                }

                case StatusType.StopBlocking:
                {
                    mainForm.ShowNotificationMessage(status.StatusType,
                        Resources.ProgramStatus_GetStatus_Blocking_stopped, status.TimeOut);
                    break;
                }

                case StatusType.ErrorException:
                {
                    mainForm.ShowNotificationMessage(status.StatusType,
                        Resources.ProgramStatus_GetStatus_Exception_was_thrown_, status.TimeOut);
                    break;
                }

                case StatusType.InfoUpToDate:
                {
                    mainForm.ShowNotificationMessage(
                        status.StatusType,
                        Resources.Your_version_is_up_to_date,
                        status.TimeOut);
                    break;
                }

                case StatusType.InfoUpdate:
                {
                    mainForm.ShowNotificationMessage(
                        status.StatusType,
                        status.AdditionalInformation,
                        status.TimeOut);

                    break;
                }

            }

            this.statusList.RemoveAt(0);
        }
    }
}
