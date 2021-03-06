import { Component, OnInit, Input } from '@angular/core';
import { BackendService } from '../../services/backend.service';
import { ContentViewModel, ContentType } from '../../models';
import { ActivatedRoute } from '@angular/router';

@Component({
    selector: 'categories',
    templateUrl: './categories.component.html',
    styleUrls: ['./categories.component.css']
})
export class CategoriesComponent implements OnInit {
    @Input()
    categories: ContentViewModel[] = [];

    inited = false;

    ngOnInit() {
        setTimeout(() => this.inited = true, 300);
    }
}
