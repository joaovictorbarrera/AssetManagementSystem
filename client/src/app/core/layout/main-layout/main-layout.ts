import { Component } from '@angular/core';
import { RouterModule } from "@angular/router";
import { Sidebar } from "./sidebar/sidebar";
import { Navbar } from "./navbar/navbar";

@Component({
  selector: 'app-main-layout',
  imports: [RouterModule, Sidebar, Navbar],
  templateUrl: './main-layout.html'
})
export class MainLayout {

}
