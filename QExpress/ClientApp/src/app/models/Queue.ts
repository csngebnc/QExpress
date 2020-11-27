// import { Category } from "../companyadmin/editcategory/editcategory.component";

import {Category} from './Category';

export interface Queue{
    id: Number,
    sorszamIdTelephelyen: Number,
    ugyfelId: String,
    telephelyId: Number,
    kategoriaId: Number,
    idopont: Date,
    allapot: String,
    sorbanAllokSzama: Number,
    telephely: String,
    kategoria: String,
    ugyfel: String,
    ceg: String,
    cegId: Number
}
