import { Category } from "../companyadmin/editcategory/editcategory.component";

export interface Queue{
    id: number;
    number: number;
    state: string;
    time: Date;
    ugyfelid: number;
    siteid: number;
    categoryid: Category;
}