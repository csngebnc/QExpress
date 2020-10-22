import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { EditUserComponent } from './edit-user/edit-user.component';
import { LoginComponent } from './login/login.component';



@NgModule({
  declarations: [EditUserComponent, LoginComponent],
  imports: [
    CommonModule
  ]
})
export class UserModule { }
