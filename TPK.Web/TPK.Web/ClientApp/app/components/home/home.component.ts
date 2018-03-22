import { Component } from '@angular/core';
import { BackendService } from '../../services/backend.service';
import { ContentViewModel, SiteViewModel } from '../../models';

@Component({
    selector: 'home',
    templateUrl: './home.component.html'
})
export class HomeComponent {
    categories: ContentViewModel[] = [];
    site: SiteViewModel = new SiteViewModel();

    constructor(private backendService: BackendService) {
    }

    ngOnInit() {
        this.backendService.getContent().then(categories => this.categories = categories);
        this.backendService.getSite().then(site => this.site = site);
    }
}
