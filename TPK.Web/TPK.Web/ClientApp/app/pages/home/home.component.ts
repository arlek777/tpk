import { Component } from '@angular/core';
import { BackendService } from '../../services/backend.service';
import { ContentViewModel} from '../../models';

@Component({
    templateUrl: './home.component.html'
})
export class HomeComponent {
    categories: ContentViewModel[] = [];

    constructor(private backendService: BackendService) {
    }

    ngOnInit() {
        this.backendService.getContent().then(categories => this.categories = categories);
    }
}
