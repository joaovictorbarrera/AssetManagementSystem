import User from "./user.dto";

export interface AssetDto {
  id: string
  assetTag: string
  name: string
  category: string
  status: string
  condition: string
  userId?: string
  userFirstName?: string,
  userLastName?: string,
  isArchived: boolean
  isPendingReturn: boolean
}
