import { Component, Input } from '@angular/core';
import { BackendService } from '../../services/backend.service';
import { ContentViewModel, ContentType } from '../../models';
import { ActivatedRoute } from '@angular/router';

@Component({
    selector: 'items',
    templateUrl: './items.component.html'
})
export class ItemsComponent {
    @Input()
    items: ContentViewModel[] = [];
}
