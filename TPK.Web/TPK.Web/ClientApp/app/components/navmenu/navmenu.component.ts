import { Component, OnInit } from '@angular/core';
import { BackendService } from '../../services/backend.service';
import { ContentViewModel, SiteViewModel } from '../../models';

@Component({
    selector: 'nav-menu',
    templateUrl: './navmenu.component.html',
    styleUrls: ['./navmenu.component.css']
})
export class NavMenuComponent implements OnInit {
    categories: ContentViewModel[] = [];
    site = new SiteViewModel();

    constructor(private backendService: BackendService) {
    }

    ngOnInit() {
        this.backendService.getContent().then(categories => this.categories = categories);
        this.backendService.getSite().then(site => this.site = site);
    }
}
