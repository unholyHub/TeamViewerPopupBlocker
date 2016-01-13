using System;

namespace TeamViewerPopupBlocker.Classes.Notifications
{
    public class Status : ICloneable
    {
        public Status()
        {
            this.StatusType = StatusType.Unknown;
            this.TimeOut = 0;
            this.TimeOutMinimum = 0;
            this.AdditionalInformation = "";
        }

        public Status(StatusType status, int timeOut, int timeOutMinimum = 0, string additionalInformation = "")
        {
            this.StatusType = status;
            this.TimeOut = timeOut;
            this.TimeOutMinimum = timeOutMinimum;
            this.AdditionalInformation = additionalInformation;
        }

        public StatusType StatusType { get; set; }

        public int TimeOut { get; set; }

        public int TimeOutMinimum { get; set; }

        public string AdditionalInformation { get; set; }

        object ICloneable.Clone()
        {
            return this.Clone();
        }

        public Status Clone()
        {
            return (Status)this.MemberwiseClone();
        }
    }
}