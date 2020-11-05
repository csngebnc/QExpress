import { User } from "./User";

export interface Company{
  id: number;
  name: string;
  admin: User;
}