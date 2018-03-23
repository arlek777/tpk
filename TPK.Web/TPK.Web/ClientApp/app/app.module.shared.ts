import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './components/app/app.component';
import { NavMenuComponent } from './components/navmenu/navmenu.component';
import { HomeComponent } from './pages/home/home.component';
import { BackendService } from './services/backend.service';
import { ContentComponent } from './pages/content/content.component';
import { CategoriesComponent } from './components/categories/categories.component';
import { ItemsComponent } from './components/items/items.component';
import { ItemDetailsComponent } from './components/item-details/item-details.component';

@NgModule({
    declarations: [
        AppComponent,
        NavMenuComponent,
        HomeComponent,
        ContentComponent,
        CategoriesComponent,
        ItemsComponent,
        ItemDetailsComponent
    ],
    imports: [
        CommonModule,
        HttpModule,
        FormsModule,
        RouterModule.forRoot([
            { path: '', redirectTo: 'home', pathMatch: 'full' },
            { path: 'home', component: HomeComponent },
            { path: 'content/:id', component: ContentComponent },
            { path: '**', redirectTo: 'home' }
        ])
    ],
    providers: [
        BackendService
    ]
})
export class AppModuleShared {
}
