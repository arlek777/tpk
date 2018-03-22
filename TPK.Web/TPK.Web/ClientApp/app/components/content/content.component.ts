import { Component, OnInit } from '@angular/core';
import { BackendService } from '../../services/backend.service';
import { ContentViewModel, ContentType } from '../../models';
import { ActivatedRoute } from '@angular/router';

@Component({
    templateUrl: './category.component.html'
})
export class CategoryComponent implements OnInit {
    content: { data: ContentViewModel[], type: ContentType }

    constructor(private backendService: BackendService, private activatedRoute: ActivatedRoute) {
    }

    ngOnInit() {
        this.backendService.getContent(this.activatedRoute.snapshot.params["id"]).then(content => {
            this.content = content;
        });
    }
}
