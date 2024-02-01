import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { ReactiveFormsModule } from '@angular/forms';

import { AppComponent } from './app.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { AddCustomerFormComponent } from './components/add-customer-form/add-customer-form.component';
import { EditCustomerFormComponent } from './components/edit-customer-form/edit-customer-form.component';

@NgModule({
  declarations: [
    AppComponent,
    AddCustomerFormComponent,
    EditCustomerFormComponent
  ],
  imports: [
    BrowserModule, HttpClientModule,
    ReactiveFormsModule,
    NgbModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
