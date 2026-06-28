import { AssetDto } from "./asset.dto";
import User from "./user.dto";

export interface CheckoutRequestDto {
  id: string;
  requestType: string;
  requestedByUserId: string;
  requestedByUser: User;
  status: string;
  assignedAssetId?: string;
  assetCategory: string;
  assignedAssetName?: string;
  assignedAssetTag?: string;
  createdAt: Date;
  isArchived: boolean;
}
