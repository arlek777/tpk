import { Component } from '@angular/core';
import { BackendService } from '../../services/backend.service';
import { SiteViewModel } from '../../models';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';

@Component({
    templateUrl: './contact.component.html'
})
export class ContactComponent {
    private mapAddressUrl = "https://www.google.com/maps/embed/v1/place?key=AIzaSyDmrOSEFeAXVcWE8M8d-hsmwQw8CnbYfcM&q=";

    site: SiteViewModel = new SiteViewModel();
    mapAddressTrustedUrl: SafeUrl;

    constructor(private backendService: BackendService, private sanitizer: DomSanitizer) {
    }

    ngOnInit() {
        this.backendService.getSite().then(site => {
            this.site = site;
            this.mapAddressTrustedUrl = this.sanitizer.bypassSecurityTrustResourceUrl(this.mapAddressUrl + this.site.address);
        });
    }
}
