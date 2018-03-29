import { Directive, ElementRef, Input, HostBinding, OnInit, OnChanges, SimpleChanges } from '@angular/core';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';

@Directive({
    selector: '[trust-html]'
})
export class TrustHtmlDirective implements OnInit, OnChanges {
    @Input("trust-html")
    trustHtmlContent: string;

    @HostBinding("innerHtml")
    bypassedHtmlContent: SafeHtml;

    constructor(private elementRef: ElementRef, private sanitizer: DomSanitizer) {
        
    }

    ngOnInit() {
        this.bypassedHtmlContent = this.sanitizer.bypassSecurityTrustHtml(this.trustHtmlContent);
    }

    ngOnChanges(changes: SimpleChanges): void {
        var trustHtmlChange = changes["trustHtmlContent"];
        if (trustHtmlChange
            && !trustHtmlChange.isFirstChange()
            && trustHtmlChange.currentValue !== trustHtmlChange.previousValue) {
            this.bypassedHtmlContent = this.sanitizer.bypassSecurityTrustHtml(trustHtmlChange.currentValue);
        }
    }
}