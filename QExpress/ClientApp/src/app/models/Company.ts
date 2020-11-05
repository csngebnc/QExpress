import { User } from "oidc-client";

export interface Company{
  id: number;
  name: string;
  admin: User;
}