import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { EditUserComponent } from './edit-user/edit-user.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';


@NgModule({
  declarations: [EditUserComponent, LoginComponent, RegisterComponent],
  imports: [
    CommonModule
  ]
})
export class UserModule { }
