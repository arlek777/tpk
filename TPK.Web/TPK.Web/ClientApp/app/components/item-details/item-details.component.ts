import { Component, Input, AfterViewInit } from '@angular/core';
import { BackendService } from '../../services/backend.service';
import { ContentViewModel, ContentType, CURRENCY } from '../../models';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
    selector: 'item-details',
    templateUrl: './item-details.component.html'
})
export class ItemDetailsComponent implements AfterViewInit {
    @Input()
    item: ContentViewModel = new ContentViewModel();

    @Input()
    items: ContentViewModel[] = [];

    currentIndex = -1;
    CURRENCY = CURRENCY;

    constructor(private router: Router) {}

    ngAfterViewInit() {
        this.currentIndex = this.items.findIndex(i => i.id === this.item.id);
    }

    next() {
        if (this.currentIndex === this.items.length - 1 || this.currentIndex == -1) return;
        this.item = this.items[++this.currentIndex];
    }

    previous() {
        if (this.currentIndex <= 0) return;
        this.item = this.items[--this.currentIndex];
    }

    navigateToPreviousUrl() {
        window.history.back();
    }
}
