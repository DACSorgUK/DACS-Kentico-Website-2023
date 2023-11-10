﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DacsOnline.Model.Manager.Interfaces;
using DacsOnline.Model.Models;
using DacsOnline.Model.RepostioriesInterfaces;
using DacsOnline.Model.Business_Objects;
using DacsOnline.Model.Utilities;
using DacsOnline.Model.Enums;
using System.Web;
using System.Configuration;
using AutoMapper;

namespace DacsOnline.Model.Manager
{
    public class ARRArtistSearchManager : BaseManager<IARRArtistSearchRepository>, IARRArtistSearchManager
    {
        #region //Private Properties
        /// <summary>
        /// 
        /// </summary>
        private List<Nationality> _listNationlity;
        #endregion

        #region //Constructor
        public ARRArtistSearchManager(IARRArtistSearchRepository ARRSearchRepository)
            : base(ARRSearchRepository)
        {
            _listNationlity = this.Repository.GetNationalities();
        }
        #endregion

        #region //Public Methods
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
        public List<ArtistARRModel> SearchArtist(string SaleYear, string ArtistFirstName, string ArtistLastName, int page, int pageSize, out int TotalItems)
        {
            List<Artist> refinedArtists = new List<Artist>();
            //List<Artist> refinedPseudonymArtists = new List<Artist>();
            List<ArtistARRModel> artistsModel = new List<ArtistARRModel>();
            //List<Artist> artistsByFirstNameAndLastName = this.Repository.GetArtistsData();
            //List<Artist> artistsByPseudonym = this.Repository.GetArtistsData();

            if ((ArtistFirstName != string.Empty || ArtistLastName != string.Empty))
            {
                //foreach (string word in SearchWords)
                //{
                //    if (!(string.IsNullOrEmpty(word.Trim()) || word.Trim() == ""))
                //    {
                string key = ConfigurationManager.AppSettings["CRMWebServiceAccessKey"].ToString();
                PayBack.DACSAccessSoapClient _service = new PayBack.DACSAccessSoapClient();
                PayBack.DACSAtristList _items = _service.SearchARRArtist(key, ArtistFirstName, ArtistLastName, pageSize, page);

                foreach(PayBack.DACSArtist obj in _items.ArtistList)
                {
                    Artist artist = new Business_Objects.Artist();

                    Mapper.CreateMap<PayBack.DACSArtist, Artist>();
                    artist = Mapper.Map<Artist>(obj);

                    if (obj.ARRConfirmedNationality.Trim() == "Confirmed")
                    {
                        artist.ARRConfirmedNationality = ARRConfirmedNationalityType.Confirmed;
                    }
                    else if (obj.ARRConfirmedNationality.Trim() == "Unconfirmed")
                    {
                        artist.ARRConfirmedNationality = ARRConfirmedNationalityType.Unconfirmed;
                    }
                    else
                    {
                        artist.ARRConfirmedNationality = ARRConfirmedNationalityType.Default;
                    }


                    if (obj.ARRMembership == "DACS")
                    {
                        artist.ARRMembership = ARRMembershipType.DACS;
                    }
                    else if (obj.ARRMembership == "SisterSociety")
                    {
                        artist.ARRMembership = ARRMembershipType.SisterSociety;
                    }
                    else if (obj.ARRMembership == "ACS")
                    {
                        artist.ARRMembership = ARRMembershipType.ACS;
                    }
                    else if (obj.ARRMembership == "ARA")
                    {
                        artist.ARRMembership = ARRMembershipType.ARA;
                    }
                    else if (obj.ARRMembership == "Rosenstiel's")
                    {
                        artist.ARRMembership = ARRMembershipType.Rosenstiel;
                    }
                    else
                    {
                        artist.ARRMembership = ARRMembershipType.NotApplicable;
                    }

                    refinedArtists.Add(artist);
                }


                //Mapper.CreateMap<PayBack.DACSArtis, Artist>()
                //  .ForMember(c => c.Id, option => option.Ignore());

                //refinedArtists = Mapper.Map<Artist>(_items.ArtistList);

                //Search based on First name and Last name
                //if (ArtistFirstName != string.Empty && ArtistLastName != string.Empty)
                //{
                //    artistsByFirstNameAndLastName = artistsByFirstNameAndLastName.Where(p => p.AuthenticFirstNames.ToUpper().Contains(ArtistFirstName.ToUpper()) || p.FirstName.ToUpper().Contains(ArtistFirstName.ToUpper())).ToList<Artist>();

                //    artistsByFirstNameAndLastName = artistsByFirstNameAndLastName.Where(p => p.AuthenticLastName.ToUpper().Contains(ArtistLastName.ToUpper()) || p.LastName.ToUpper().Contains(ArtistLastName.ToUpper())).ToList<Artist>();
                //}
                //else if (ArtistFirstName != string.Empty)
                //{
                //    artistsByFirstNameAndLastName = artistsByFirstNameAndLastName.Where(p => p.AuthenticFirstNames.ToUpper().Contains(ArtistFirstName.ToUpper()) || p.FirstName.ToUpper().Contains(ArtistFirstName.ToUpper())).ToList<Artist>();
                //}
                //else if (ArtistLastName != string.Empty)
                //{
                //    artistsByFirstNameAndLastName = artistsByFirstNameAndLastName.Where(p => p.AuthenticLastName.ToUpper().Contains(ArtistLastName.ToUpper()) || p.LastName.ToUpper().Contains(ArtistLastName.ToUpper())).ToList<Artist>();
                //}
                //else
                //{
                //}

                //artistsByFirstNameAndLastName = artistsByFirstNameAndLastName.Select(x => { x.Relevence = "1"; return x; }).ToList<Artist>();

                //refinedArtists.AddRange(artistsByFirstNameAndLastName);

                //if (ArtistFirstName != string.Empty)
                //{
                //    refinedPseudonymArtists.AddRange(artistsByPseudonym.Where(p =>
                //                 p.Pseudonym_1.ToUpper().Contains(ArtistFirstName.ToUpper()) ||
                //                 p.Pseudonym_2.ToUpper().Contains(ArtistFirstName.ToUpper()) ||
                //                 p.Pseudonym_3.ToUpper().Contains(ArtistFirstName.ToUpper()) ||
                //                 p.Pseudonym_4.ToUpper().Contains(ArtistFirstName.ToUpper()) ||
                //                 p.Pseudonym_5.ToUpper().Contains(ArtistFirstName.ToUpper()) ||
                //                 p.Pseudonym_6.ToUpper().Contains(ArtistFirstName.ToUpper())
                //                 ).ToList<Artist>());
                //}
                //if (ArtistLastName != string.Empty)
                //{
                //    refinedPseudonymArtists.AddRange(artistsByPseudonym.Where(p =>
                //                 p.Pseudonym_1.ToUpper().Contains(ArtistLastName.ToUpper()) ||
                //                 p.Pseudonym_2.ToUpper().Contains(ArtistLastName.ToUpper()) ||
                //                 p.Pseudonym_3.ToUpper().Contains(ArtistLastName.ToUpper()) ||
                //                 p.Pseudonym_4.ToUpper().Contains(ArtistLastName.ToUpper()) ||
                //                 p.Pseudonym_5.ToUpper().Contains(ArtistLastName.ToUpper()) ||
                //                 p.Pseudonym_6.ToUpper().Contains(ArtistLastName.ToUpper())
                //                 ).ToList<Artist>());
                //}

                //refinedPseudonymArtists = refinedPseudonymArtists.Distinct(new DistinctArtistId()).ToList<Artist>();

                //foreach (Artist obj in artistsByFirstNameAndLastName)
                //{
                //    Artist remove = refinedPseudonymArtists.Where(p => p.ArtistId == obj.ArtistId).FirstOrDefault();

                //    if (remove != null)
                //    {
                //        refinedPseudonymArtists.Remove(remove);
                //    }
                //}

                //refinedPseudonymArtists = refinedPseudonymArtists.Select(x => { x.Relevence = "2"; return x; }).ToList<Artist>();

                // refinedArtists.AddRange(refinedPseudonymArtists);

                //refinedArtists = refinedArtists.Distinct(new DistinctArtistId()).ToList<Artist>();
                //  refinedArtists = refinedArtists.OrderBy(p => p.Relevence).ThenBy(p => p.LastName).ThenBy(p => p.FirstName).ToList<Artist>();
                //  TotalItems = refinedArtists.Count();
                //  refinedArtists = GetCurrentPageData(refinedArtists, pageSize, page);

                TotalItems = _items.TotalArtist;
                foreach (Artist obj in refinedArtists)
                {
                    ArtistARRModel objModel = new ArtistARRModel();
                    objModel.ArtistId = obj.ArtistId;
                    objModel.FirstName = obj.FirstName;
                    objModel.LastName = obj.LastName;
                    objModel.Nationality = obj.GetNationality(_listNationlity);
                    objModel.YearOfBirth = GetBirthYear(obj);
                    objModel.YearOfDeath = GetDeathYear(obj);
                    objModel.Confirmed = obj.ARRConfirmedNationality == ARRConfirmedNationalityType.Confirmed ? ARRConfirmedNationalityType.Confirmed.ToString() : ARRConfirmedNationalityType.Unconfirmed.ToString();
                    SetMessages(obj, objModel, SaleYear);
                    artistsModel.Add(objModel);
                }

                return artistsModel;
            }
            else
            {
                TotalItems = 0;
                return artistsModel;
            }
        }

        /// <summary>
        /// Gets the sales years.
        /// </summary>
        /// <returns></returns>
        public List<string> GetSalesYears()
        {
            return this.Repository.GetSalesYears();
        }


        /// <summary>
        /// Sets the messages.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="modObj">The mod obj.</param>
        /// <param name="SaleYear">The sale year.</param>
        public void SetMessages(Artist obj, ArtistARRModel modObj, string SaleYear)
        {
            bool saleYear = IsArtistSeventyYears(obj, SaleYear);
            if (saleYear)
            {
                if (obj.ARRMembership == ARRMembershipType.DACS)
                {
                    SetMessageWhenUserMemberShip_DACS(obj, modObj);

                }
                else if (obj.ARRMembership == ARRMembershipType.SisterSociety)
                {
                    SetMessageWhenUserMemberShip_SisterSociety(obj, modObj);
                }
                else if (obj.ARRMembership == ARRMembershipType.ACS || obj.ARRMembership == ARRMembershipType.ARA || obj.ARRMembership == ARRMembershipType.Rosenstiel)
                {
                    SetMessageWhenUserMemberShip_ACS_ARA_Rosenstiel(obj, modObj);
                }
                else
                {
                    SetMessageWhenUserMemberShip_Blank(obj, modObj);
                }
            }
            else//if the artist is outside of the 70 years
            {
                modObj.EligibilityMessage = GetMessage(ConstantDataArtistSearch.EligibilityMessage_6);
                modObj.DisplayArr = false;
            }
        }


        #endregion

        #region //Private Methods


        /// <summary>
        /// Gets the current page data.
        /// </summary>
        /// <param name="artists">The artists.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="page">The page.</param>
        /// <returns></returns>
        private List<Artist> GetCurrentPageData(List<Artist> artists, int pageSize, int page)
        {
            int take = page * pageSize;
            int skip = page == 1 ? 0 : take - pageSize;
            return artists.Take(take).Skip(skip).ToList<Artist>();
        }





        /// <summary>
        /// Gets the message.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        private string GetMessage(string key)
        {
            return HttpContext.GetGlobalResourceObject(ConstantDataArtistSearch.ArtistResourceFile, key) as string;
        }


        /// <summary>
        /// Sets the message when user DACS.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="modObj">The mod obj.</param>
        private void SetMessageWhenUserMemberShip_DACS(Artist obj, ArtistARRModel modObj)
        {
            ArtistNationality isEEA = IsUserEEA(obj);
            if (isEEA == ArtistNationality.EEA)
            {
                if (obj.ARRConfirmedNationality == ARRConfirmedNationalityType.Confirmed)
                {
                    modObj.EligibilityMessage = GetMessage(ConstantDataArtistSearch.EligibilityMessage_1);// "ARR payments are necessary";
                    modObj.MandateMessage = GetMessage(ConstantDataArtistSearch.MandateMessage_1); //"Verified by DACS as eligible for ARR Royalties";
                    modObj.PaymentMessage = GetMessage(ConstantDataArtistSearch.PaymentMessage_1); //"This artist has directly mandated dacs to represent them for ARR.";
                    modObj.DisplayArr = true;
                }
                else
                {
                    modObj.EligibilityMessage = GetMessage(ConstantDataArtistSearch.EligibilityMessage_1); //"ARR payments are necessary";
                    modObj.MandateMessage = string.Empty;
                    modObj.PaymentMessage = GetMessage(ConstantDataArtistSearch.PaymentMessage_1); //"This artist has directly mandated dacs to represent them for ARR.";
                    modObj.DisplayArr = true;
                }
            }
            else
            {
                if (obj.ARRConfirmedNationality == ARRConfirmedNationalityType.Confirmed)
                {
                    modObj.EligibilityMessage = GetMessage(ConstantDataArtistSearch.EligibilityMessage_2); ;// "ARR payments are not necessary for this artist due to their nationality";
                    modObj.MandateMessage = string.Empty;
                    modObj.PaymentMessage = string.Empty;
                    modObj.DisplayArr = false;
                }
                else
                {
                    modObj.EligibilityMessage = GetMessage(ConstantDataArtistSearch.EligibilityMessage_2); //"ARR payments are not necessary for this artist due to their nationality";
                    modObj.MandateMessage = string.Empty;
                    modObj.PaymentMessage = string.Empty;
                    modObj.DisplayArr = false;
                }
            }
        }

        /// <summary>
        /// Sets the message when user sister society.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="modObj">The mod obj.</param>
        private void SetMessageWhenUserMemberShip_SisterSociety(Artist obj, ArtistARRModel modObj)
        {
            ArtistNationality isEEA = IsUserEEA(obj);
            if (isEEA == ArtistNationality.EEA)
            {
                if (obj.ARRConfirmedNationality == ARRConfirmedNationalityType.Confirmed)
                {
                    modObj.EligibilityMessage = GetMessage(ConstantDataArtistSearch.EligibilityMessage_1); //"ARR Payments are necessary";
                    modObj.MandateMessage = GetMessage(ConstantDataArtistSearch.MandateMessage_1); //"Verified by DACS as eligible for ARR Royalties";
                    modObj.PaymentMessage = GetMessage(ConstantDataArtistSearch.PaymentMessage_2); //"DACS is acting as the collecting agency through a Sister Society";
                    modObj.DisplayArr = true;
                }
                else
                {
                    modObj.EligibilityMessage = GetMessage(ConstantDataArtistSearch.EligibilityMessage_1); //"ARR payments may be necessary";
                    modObj.MandateMessage = GetMessage(ConstantDataArtistSearch.MandateMessage_2); //"DACS has not confirmed this artists nationality";
                    modObj.PaymentMessage = GetMessage(ConstantDataArtistSearch.PaymentMessage_2); //"DACS is acting as the collecting agency through a Sister Society";
                    modObj.DisplayArr = true;
                }
            }
            else
            {
                if (obj.ARRConfirmedNationality == ARRConfirmedNationalityType.Confirmed)
                {
                    modObj.EligibilityMessage = GetMessage(ConstantDataArtistSearch.EligibilityMessage_2); //"ARR payments are not necessary for this artist due to their nationality";
                    modObj.MandateMessage = string.Empty;
                    modObj.PaymentMessage = GetMessage(ConstantDataArtistSearch.PaymentMessage_2); //"DACS is acting as the collecting agency through a Sister Society";
                    modObj.DisplayArr = false;
                }
                else
                {
                    modObj.EligibilityMessage = GetMessage(ConstantDataArtistSearch.EligibilityMessage_3);// "ARR payments may be necessary";
                    modObj.MandateMessage = GetMessage(ConstantDataArtistSearch.MandateMessage_2); //"DACS has not confirmed this artists nationality";
                    modObj.PaymentMessage = GetMessage(ConstantDataArtistSearch.PaymentMessage_2); //"DACS is acting as the collecting agency through a Sister Society";
                    modObj.DisplayArr = true;

                }
            }
        }

        /// <summary>
        /// Sets the message when user_ AC s_ AR a_ ARA.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="modObj">The mod obj.</param>
        private void SetMessageWhenUserMemberShip_ACS_ARA_Rosenstiel(Artist obj, ArtistARRModel modObj)
        {
            ArtistNationality isEEA = IsUserEEA(obj);

            string arrMembershipData = obj.ARRMembership.ToString();

            if (obj.ARRMembership == ARRMembershipType.Rosenstiel)
            {
                arrMembershipData = arrMembershipData + "'s";
            }


            if (isEEA == ArtistNationality.EEA)
            {
                if (obj.ARRConfirmedNationality == ARRConfirmedNationalityType.Confirmed)
                {
                    modObj.EligibilityMessage = GetMessage(ConstantDataArtistSearch.EligibilityMessage_4); //"ARR payments may be necessary";
                    modObj.MandateMessage = string.Empty;
                    modObj.PaymentMessage = GetMessage(ConstantDataArtistSearch.PaymentMessage_3) + " " + arrMembershipData;
                    modObj.DisplayArr = true;

                }
                else
                {
                    modObj.EligibilityMessage = GetMessage(ConstantDataArtistSearch.EligibilityMessage_4); //"ARR payments may be necessary";
                    modObj.MandateMessage = string.Empty;
                    modObj.PaymentMessage = GetMessage(ConstantDataArtistSearch.PaymentMessage_3) + " " + arrMembershipData;
                    modObj.DisplayArr = true;
                }
            }
            else
            {
                if (obj.ARRConfirmedNationality == ARRConfirmedNationalityType.Confirmed)
                {
                    modObj.EligibilityMessage = GetMessage(ConstantDataArtistSearch.EligibilityMessage_4); //"ARR payments may be necessary";
                    modObj.MandateMessage = string.Empty;
                    modObj.PaymentMessage = GetMessage(ConstantDataArtistSearch.PaymentMessage_3) + " " + arrMembershipData;
                    modObj.DisplayArr = true;
                }
                else
                {
                    modObj.EligibilityMessage = GetMessage(ConstantDataArtistSearch.EligibilityMessage_4); //"ARR payments may be necessary";
                    modObj.MandateMessage = string.Empty;
                    modObj.PaymentMessage = GetMessage(ConstantDataArtistSearch.PaymentMessage_3) + " " + arrMembershipData;
                    modObj.DisplayArr = true;
                }
            }
        }


        /// <summary>
        /// Sets the message when user member ship_ blank.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="modObj">The mod obj.</param>
        private void SetMessageWhenUserMemberShip_Blank(Artist obj, ArtistARRModel modObj)
        {
            ArtistNationality isEEA = IsUserEEA(obj);
            if (isEEA == ArtistNationality.EEA)
            {
                if (obj.ARRConfirmedNationality == ARRConfirmedNationalityType.Confirmed)
                {
                    if (obj.ARRPaidRoyalties == ARRPaidRoyalties.Yes)
                    {
                        modObj.EligibilityMessage = GetMessage(ConstantDataArtistSearch.EligibilityMessage_1); //"ARR payments are necessary";
                        modObj.MandateMessage = string.Empty;
                        modObj.PaymentMessage = GetMessage(ConstantDataArtistSearch.PaymentMessage_4);// "DACS has paid royalties to this artist";
                        modObj.DisplayArr = true;
                    }
                    else
                    {
                        modObj.EligibilityMessage = GetMessage(ConstantDataArtistSearch.EligibilityMessage_1); //"ARR payments are necessary";
                        modObj.MandateMessage = string.Empty;
                        modObj.PaymentMessage = GetMessage(ConstantDataArtistSearch.PaymentMessage_4);//"DACS pays royalties to this artist";
                        modObj.DisplayArr = true;
                    }

                }
                else
                {
                    if (obj.ARRPaidRoyalties == ARRPaidRoyalties.Yes)
                    {
                        modObj.EligibilityMessage = GetMessage(ConstantDataArtistSearch.EligibilityMessage_4); //"ARR payments may be necessary";
                        modObj.MandateMessage = GetMessage(ConstantDataArtistSearch.MandateMessage_2); //"DACS has not confirmed this artists nationality";
                        modObj.DisplayArr = true;
                    }
                    else
                    {
                        modObj.EligibilityMessage = GetMessage(ConstantDataArtistSearch.EligibilityMessage_4); //"ARR payments may be necessary";
                        modObj.MandateMessage = GetMessage(ConstantDataArtistSearch.MandateMessage_2); //"DACS has not confirmed this artists nationality";
                        modObj.DisplayArr = true;
                    }
                }
            }
            else if (isEEA == ArtistNationality.NonEEA)
            {
                if (obj.ARRConfirmedNationality == ARRConfirmedNationalityType.Confirmed)
                {
                    if (obj.ARRPaidRoyalties == ARRPaidRoyalties.Yes)
                    {
                        modObj.EligibilityMessage = GetMessage(ConstantDataArtistSearch.EligibilityMessage_2); //"ARR payments are not necessary for this artist due to their nationality";
                        modObj.MandateMessage = string.Empty;
                        modObj.PaymentMessage = GetMessage(ConstantDataArtistSearch.PaymentMessage_4);//"DACS has paid royalties to this artist";
                        modObj.DisplayArr = false;
                    }
                    else
                    {
                        modObj.EligibilityMessage = GetMessage(ConstantDataArtistSearch.EligibilityMessage_2); //"ARR payments are not necessary for this artist due to their nationality";
                        modObj.MandateMessage = string.Empty;
                        modObj.PaymentMessage = string.Empty;
                        modObj.DisplayArr = false;
                    }
                }
                else
                {
                    if (obj.ARRPaidRoyalties == ARRPaidRoyalties.Yes)
                    {
                        modObj.EligibilityMessage = GetMessage(ConstantDataArtistSearch.EligibilityMessage_3); //"ARR payments may be necessary";
                        modObj.MandateMessage = GetMessage(ConstantDataArtistSearch.MandateMessage_2); //"DACS has not confirmed this artists nationality";
                        modObj.PaymentMessage = GetMessage(ConstantDataArtistSearch.PaymentMessage_4);//"DACS has paid royalties to this artist";
                        modObj.DisplayArr = true;
                    }
                    else
                    {
                        modObj.EligibilityMessage = GetMessage(ConstantDataArtistSearch.EligibilityMessage_3); //"ARR payments may be necessary";
                        modObj.MandateMessage = GetMessage(ConstantDataArtistSearch.MandateMessage_2); //"DACS has not confirmed this artists nationality";
                                                                                                       //  modObj.PaymentMessage = GetMessage(ConstantDataArtistSearch.PaymentMessage_4);//"DACS has paid royalties to this artist";
                        modObj.DisplayArr = true;
                    }
                }
            }
            else
            {
                modObj.EligibilityMessage = GetMessage(ConstantDataArtistSearch.EligibilityMessage_3); //"ARR payments may be necessary";
                modObj.MandateMessage = GetMessage(ConstantDataArtistSearch.MandateMessage_2); //"DACS has not confirmed this artists nationality";
                modObj.PaymentMessage = string.Empty;
                modObj.DisplayArr = true;
            }
        }


        /// <summary>
        /// Determines whether [is artist seventy years] [the specified obj].
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>
        ///   <c>true</c> if [is artist seventy years] [the specified obj]; otherwise, <c>false</c>.
        /// </returns>
        private bool IsArtistSeventyYears(Artist obj, string SaleYear)
        {
            try
            {
                if (obj.DateOfDeath != null)
                {
                    int diff = Convert.ToInt32(SaleYear) - obj.DateOfDeath.Value.Year;
                    if (diff > 70)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else if (obj.YearOfDeath != null)
                {

                    int diff = Convert.ToInt32(SaleYear) - Convert.ToInt32(obj.YearOfDeath);
                    if (diff > 70)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }

                }
                else
                {
                    return true;
                }

            }
            catch
            {
                return true;
            }
        }


        /// <summary>
        /// Determines whether [is user EEA] [the specified obj].
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns></returns>
        private ArtistNationality IsUserEEA(Artist obj)
        {
            List<string> eeaList = _listNationlity
                                  .Where(p => p.EEA == true)
                                  .Select(p => p.Country.Trim()).ToList<string>();
            if (string.IsNullOrEmpty(obj.Nationality1.Trim()) &&
                string.IsNullOrEmpty(obj.Nationality2.Trim()) &&
                string.IsNullOrEmpty(obj.Nationality3.Trim()) &&
                string.IsNullOrEmpty(obj.Nationality4.Trim()))
            {
                return ArtistNationality.NotDefined;
            }
            else
            {

                if (eeaList.Contains(obj.Nationality1.Trim()) ||
                       eeaList.Contains(obj.Nationality2.Trim()) ||
                       eeaList.Contains(obj.Nationality3.Trim()) ||
                       eeaList.Contains(obj.Nationality4.Trim()))
                {
                    return ArtistNationality.EEA;
                }
                else
                {
                    return ArtistNationality.NonEEA;
                }
            }

        }

        /// <summary>
        /// Gets the birth year.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns></returns>
        private string GetBirthYear(Artist obj)
        {
            if (obj.DateOfBirth != null)
            {
                return obj.DateOfBirth.Value.Year.ToString();
            }
            else if (obj.YearOfBirth != null)
            {
                return obj.YearOfBirth;

            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets the death year.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns></returns>
        private string GetDeathYear(Artist obj)
        {
            if (obj.DateOfDeath != null)
            {
                return obj.DateOfDeath.Value.Year.ToString();
            }
            else if (obj.YearOfDeath != null)
            {
                return obj.YearOfDeath;

            }
            else
            {
                return string.Empty;
            }
        }
        #endregion


    }


}
