import { Component, Input } from '@angular/core';
import { BackendService } from '../../services/backend.service';
import { ContentViewModel, ContentType } from '../../models';
import { ActivatedRoute } from '@angular/router';

@Component({
    selector: 'item-details',
    templateUrl: './item-details.component.html'
})
export class ItemDetailsComponent {
    @Input()
    item: ContentViewModel = new ContentViewModel();
}
