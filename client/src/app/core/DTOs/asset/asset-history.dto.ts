import UserDto from "../user/user.dto"

export default interface AssetHistory {
    id: string,
    assetId: string,
    userId: string,
    user: UserDto,
    action: string,
    oldValue?: string,
    newValue?: string,
    createdAt: string
}
