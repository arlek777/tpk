import { Component, OnInit } from '@angular/core';
import { BackendService } from '../../services/backend.service';
import { ContentViewModel, ContentType } from '../../models';
import { ActivatedRoute } from '@angular/router';

@Component({
    templateUrl: './content.component.html'
})
export class ContentComponent implements OnInit {
    content: { data: ContentViewModel[], type: ContentType, item: ContentViewModel };
    ContentType = ContentType;

    constructor(private backendService: BackendService, private activatedRoute: ActivatedRoute) {
    }

    ngOnInit() {
        this.activatedRoute.params.subscribe(params => {
            this.backendService.getContent(params["id"]).then(content => {
                this.content = content;
            });
        });
    }
}
