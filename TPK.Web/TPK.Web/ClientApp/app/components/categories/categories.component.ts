import { Component, OnInit, Input } from '@angular/core';
import { BackendService } from '../../services/backend.service';
import { ContentViewModel, ContentType } from '../../models';
import { ActivatedRoute } from '@angular/router';

@Component({
    selector: 'categories',
    templateUrl: './categories.component.html'
})
export class CategoriesComponent {
    @Input()
    categories: ContentViewModel[] = [];
}
