using MindSageWeb.Repositories.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindSageWeb.Repositories
{
    /// <summary>
    /// มาตรฐานในการติดต่อกับ Friend request
    /// </summary>
    public interface IImageUploader
    {
        #region Methods

        /// <summary>
        /// upload image to azure and get That's Path URL 
        /// </summary>
        /// <param name="ImagePath">Image's Path</param>
        string Upload(Stream ImagePath);

        #endregion Methods#region Methods
    }
}
