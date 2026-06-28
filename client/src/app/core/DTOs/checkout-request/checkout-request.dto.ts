import UserDto from "../user/user.dto";

export interface CheckoutRequestDto {
  id: string;
  requestType: string;
  requestedByUserId: string;
  requestedByUser: UserDto;
  status: string;
  assignedAssetId?: string;
  assetCategory: string;
  assignedAssetName?: string;
  assignedAssetTag?: string;
  createdAt: Date;
  isArchived: boolean;
}
