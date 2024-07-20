using System.Collections.Generic;
using System.Linq;

namespace MainTainSense.Data.Repositories
{
    public class AssetRepository : IAssetRepository
    {
        private readonly MainTainSenseDbContext _context;

        public AssetRepository(MainTainSenseDbContext context)
        {
            _context = context;
        }

        public List<Assets> GetAllAssets()
        {
            return _context.Assets.ToList();
        }

        public Assets GetAssetById(int assetId)
        {
            return _context.Assets.Find(assetId);
        }

        public void AddAsset(Assets asset)
        {
            _context.Assets.Add(asset);
            _context.SaveChanges();
        }

        public void UpdateAsset(Assets asset)
        {
            var existingAsset = _context.Assets.Find(asset.Id); // Assuming 'Id' is your primary key
            if (existingAsset != null)
            {
                // Copy values from 'asset' into 'existingAsset'
                _context.Entry(existingAsset).CurrentValues.SetValues(asset);
            }
            _context.SaveChanges();
        }

        public void DeleteAsset(int assetId)
        {
            Assets asset = _context.Assets.Find(assetId);
            if (asset != null)
            {
                _context.Assets.Remove(asset);
                _context.SaveChanges();
            }
        }

        public Assets GetAssetByAssetType(string assetType)
        {
            return _context.Assets.FirstOrDefault(a => a.AssetType.AssetTypeName == assetType);
        }

        public Assets GetAssetByLocation(string location)
        {
            return _context.Assets.FirstOrDefault(a => a.Location.LocationName == location);
        }

        public Assets GetAssetByStatus(string status)
        {
            return _context.Assets.FirstOrDefault(a => a.Status.StatusName == status);
        }

        public Assets GetAssetByIsActive(int isActive)
        {
            bool activeState = isActive == 1; 
            return _context.Assets.FirstOrDefault(a => a.IsActive == activeState);
        }
    }
}

