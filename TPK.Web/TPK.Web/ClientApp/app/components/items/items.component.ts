import { Component, Input, OnInit } from '@angular/core';
import { BackendService } from '../../services/backend.service';
import { ContentViewModel, ContentType, CURRENCY } from '../../models';
import { ActivatedRoute } from '@angular/router';

@Component({
    selector: 'items',
    templateUrl: './items.component.html'
})
export class ItemsComponent implements OnInit {
    @Input()
    items: ContentViewModel[] = [];

    inited = false;
    CURRENCY = CURRENCY;

    ngOnInit() {
        setTimeout(() => this.inited = true, 500);
    }
}
