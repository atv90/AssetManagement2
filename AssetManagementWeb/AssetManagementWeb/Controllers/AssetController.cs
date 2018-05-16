using AssetManagementWeb.Database;
using AssetManagementWeb.Models;
using AssetManagementWeb.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AssetManagementWeb.Controllers
{
    public class AssetController : Controller
    {
        // GET: Asset
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Test()
        {
            return View();
        }
        
        //DAY2
        public  ActionResult List()
        {
            //listan muodostus
            List<LocatedAssetsViewModel> model = new List<LocatedAssetsViewModel>();

            //TIETOKANTAYHTEYS
            Asset2Entities entities = new Asset2Entities();
            try
            {
                //haetaan tietoja tietokannasta
                List<AssetLocations> assets = entities.AssetLocations.ToList();
                //muodostetaan näkymämalli tietokannan rivien pohjalta
                CultureInfo fiFi = new CultureInfo("fi-Fi");
                foreach (AssetLocations asset in assets)
                {
                    LocatedAssetsViewModel view = new LocatedAssetsViewModel();
                    view.Id = asset.Id;
                    view.LocationCode = asset.AssetLocation.Code;
                    view.LocationName = asset.AssetLocation.Name;
                    view.AssetCode = asset.Assets.Code;
                    view.AssetName = asset.Assets.Type + ": " + asset.Assets.Model;
                    view.LastSeen = asset.LastSeen.Value.ToString(fiFi);
                    
                    model.Add(view);
                }

            }
            //muistinvapautus
            finally
            {
                entities.Dispose();
            }
            //palautetaan ylempänä luotu model-niminen lista
            return View(model);
        }
        //DAY1
        [HttpPost]
        public JsonResult AssignLocation()
        {

            string json = Request.InputStream.ReadToEnd();
            AssignLocationModel inputData =
            JsonConvert.DeserializeObject<AssignLocationModel>(json);

            bool success = false;
            string error = "";
            Asset2Entities entities = new Asset2Entities();
            try
            {
                // haetaan ensin paikan id-numero koodin perusteella
                int locationId = (from l in entities.AssetLocation
                                  where l.Code == inputData.LocationCode
                                  select l.Id).FirstOrDefault();

                // haetaan laitteen id-numero koodin perusteella
                int assetId = (from a in entities.Assets
                               where a.Code == inputData.AssetCode
                               select a.Id).FirstOrDefault();

                if ((locationId > 0) && (assetId > 0))
                {
                    // UUDEN RIVIN TALLENTAMINEN aikaleiman kanssa TIETOKANTAAN
                    AssetLocations newEntry = new AssetLocations();
                    newEntry.LocationId = locationId;
                    newEntry.AssetId = assetId;
                    newEntry.LastSeen = DateTime.Now;

                    entities.AssetLocations.Add(newEntry);
                    entities.SaveChanges();

                    success = true;
                }
            }
            catch (Exception ex)
            {
                error = ex.GetType().Name + ": " + ex.Message;
            }
            finally
            {
                entities.Dispose();
            }

            // palautetaan JSON-muotoinen tulos kutsujalle
            var result = new { success = success, error = error };
            return Json(result);
        }

        
    }
}
