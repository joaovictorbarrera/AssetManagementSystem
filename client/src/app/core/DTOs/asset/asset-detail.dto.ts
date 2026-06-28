export interface AssetDetailDto {
  id: string
  assetTag: string
  name: string
  serialNumber: string
  category: string
  status: string
  condition: string
  userId?: string
  userFirstName?: string,
  userLastName?: string,
  updatedAt: string,
  createdAt: string,
  isPendingReturn: boolean
  isArchived: boolean
}
