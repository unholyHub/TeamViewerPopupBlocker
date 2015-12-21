//-----------------------------------------------------------------------
// <copyright file="WebClientExtent.cs" company="Zhivko Kabaivanov">
//     Copyright (c) Zhivko Kabaivanov. All rights reserved.
// </copyright>
// <author>Zhivko Kabaivanov</author>
//-----------------------------------------------------------------------
namespace TeamViewerPopupBlocker.Classes
{
    using System;
    using System.Net;

    /// <summary>
    /// Extension method for the <see cref="WebClient"/>.
    /// </summary>
    public class WebClientExtent : WebClient
    {
        /// <summary>
        /// Field storing the timeout in milliseconds.
        /// </summary>
        private int timeout;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebClientExtent"/> class.
        /// </summary>
        /// <param name="timeout">The timeout value in milliseconds.</param>
        public WebClientExtent(int timeout) : this()
        {
            this.timeout = timeout;
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="WebClientExtent"/> class from being created.
        /// </summary>
        private WebClientExtent()
        {
            this.timeout = 10 * 1000;

            this.Proxy = null;
            this.Credentials = CredentialCache.DefaultCredentials;
        }

        /// <summary>
        /// Gets or sets the time-out value in milliseconds.
        /// </summary>
        public int Timeout
        {
            get
            {
                return this.timeout;
            }

            set
            {
                this.timeout = value;
            }
        }

        /// <summary>
        /// Returns a <see cref="WebRequest"/> object for the specified resource.
        /// </summary>
        /// <param name="address"> A <see cref="Uri"/> that identifies the resource to request</param>
        /// <returns> A new <see cref="WebRequest"/> object for the specified resource.</returns>
        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest result = base.GetWebRequest(address);

            if (result != null)
            {
                result.Timeout = this.timeout;
                return result;
            }

            return null;
        }    
    }
}
