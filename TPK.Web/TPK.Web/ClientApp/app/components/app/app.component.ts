import { Component } from '@angular/core';
import { BackendService } from '../../services/backend.service';
import { SiteViewModel } from '../../models';

@Component({
    selector: 'app',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css']
})
export class AppComponent {
    site: SiteViewModel = new SiteViewModel();

    constructor(private backendService: BackendService) { }

    ngOnInit() {
        this.backendService.getSite().then(site => this.site = site);
    }
}
