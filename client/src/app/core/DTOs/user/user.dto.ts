import { Role } from "../../enums/role";

export default interface UserDto {
    id: string,
    emailAddress: string,
    role: Role,
    lastLoginAt?: string,
    isActive: boolean,
    createdAt: string,
    firstName: string,
    lastName: string
}
