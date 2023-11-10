﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DacsOnline.Model.Models;
using DacsOnline.Model.Business_Objects;

namespace DacsOnline.Model.Manager.Interfaces
{
    public interface ICLArtistSearchManager
    {

       /// <summary>
       /// Searches the artist.
       /// </summary>
       /// <param name="SaleYear">The sale year.</param>
       /// <param name="ArtistFirstName">First name of the artist.</param>
       /// <param name="ArtistLastName">Last name of the artist.</param>
       /// <param name="page">The page.</param>
       /// <param name="pageSize">Size of the page.</param>
       /// <param name="TotalItems">The total items.</param>
       /// <returns></returns>
        List<ArtistCLModel> SearchArtist(string SaleYear, string ArtistFirstName, string ArtistLastName, int page, int pageSize, out int TotalItems, string dataSource);

        /// <summary>
        /// Sets the messages.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="modObj">The mod obj.</param>
        /// <param name="Year">The year.</param>
        void SetMessages(Artist obj, ArtistCLModel modObj, string Year);
    }
}
