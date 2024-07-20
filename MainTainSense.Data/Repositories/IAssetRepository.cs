using System.Collections.Generic;

namespace MainTainSense.Data.Repositories
{
    public interface IAssetRepository
    {
        List<Assets> GetAllAssets();
        Assets GetAssetById(int assetId);
        void AddAsset(Assets asset);
        void UpdateAsset(Assets asset);
        void DeleteAsset(int assetId);
        Assets GetAssetByAssetType(string assetType);
        Assets GetAssetByLocation(string location);
        Assets GetAssetByStatus(string status);
        Assets GetAssetByIsActive(int isActive);
    }
}
