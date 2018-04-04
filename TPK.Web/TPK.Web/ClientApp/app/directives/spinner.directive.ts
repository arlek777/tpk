import { Directive, ElementRef, Renderer, OnInit, Input, OnChanges, SimpleChanges } from '@angular/core';

@Directive({
    selector: '[spinner]'
})
export class SpinnerDirective implements OnChanges {
    private loadingElement: any;

    @Input("spinner")
    spinnerPredicate: boolean;

    constructor(private elementRef: ElementRef, private renderer: Renderer) {
        this.loadingElement = this.renderer.selectRootElement("#spinner");
        this.loadingElement.innerHTML = "Загрузка..";
    }

    ngOnChanges(changes: SimpleChanges) {
        var spinnerChange = changes["spinnerPredicate"];
        if (!spinnerChange.currentValue) {
            this.loadingElement.classList.remove("hidden");
            this.elementRef.nativeElement.classList.add("hidden");
        } else {
            this.loadingElement.classList.add("hidden");
            this.elementRef.nativeElement.classList.remove("hidden");
        }
    }

}