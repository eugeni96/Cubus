﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adam.Core.Records;

namespace Coub.UI.Models
{
    public class MediaSetViewModel
    {
        public List<MediaViewModel> MediaViewModels { get; set; }

        public MediaSetViewModel(RecordCollection recordCollection)
        {
            MediaViewModels=new List<MediaViewModel>();
            foreach (Record rec in recordCollection)
            {
                MediaViewModels.Add(new MediaViewModel(rec));
            }
        }
    }
}