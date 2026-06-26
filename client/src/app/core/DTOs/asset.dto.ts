import User from "./user.dto";

export interface Asset {
  id: string;
  assetTag: string;
  name: string;
  category: string;
  serialNumber?: string | null;
  status: string;
  condition: string;
  assignedToUserId?: string | null;
  assignedToUser?: User | null;
  updatedAt?: string | null;
  isArchived: boolean;
  createdAt: string;
}
