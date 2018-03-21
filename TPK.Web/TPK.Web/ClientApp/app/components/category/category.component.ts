import { Component, OnInit } from '@angular/core';
import { BackendService } from '../../services/backend.service';

@Component({
    templateUrl: './category.component.html'
})
export class CategoryComponent implements OnInit {


    constructor(private backendService: BackendService) {
    }

    ngOnInit() {
        
    }
}
