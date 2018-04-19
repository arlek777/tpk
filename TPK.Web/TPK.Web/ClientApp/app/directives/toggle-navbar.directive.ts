import { Directive, ElementRef, Renderer, OnInit, Input } from '@angular/core';

@Directive({
    selector: '[toggle-mobile-navbar]'
})
export class ToggleMobileNavbarDirective implements OnInit {
    private nativeElement: any;
    private navbar: any;

    private isNavbarOpened = false;
    private isSmallScreen = false;

    private readonly closeCssClass = "closed";
    private readonly animationSliderCssClass = "slider";

    constructor(private elementRef: ElementRef, private renderer: Renderer) {
        this.nativeElement = elementRef.nativeElement;
    }

    ngOnInit() {
        this.navbar = this.nativeElement.querySelector("div[class='navbar-collapse']");

        if (window.innerWidth <= 760) {
            this.setSmallScreenCss();
        }
        this.addWindowResizeEvent();
        this.addNavbarClickEvent();
        this.addToggleButtonEvent();
    }

    private setSmallScreenCss() {
        this.hideNavbar();
        this.navbar.classList.add(this.animationSliderCssClass);
        this.isSmallScreen = true;
    }

    private addWindowResizeEvent() {
        this.renderer.listenGlobal("window", "resize", (e: any) => {
            if (e.target.innerWidth > 760) {
                this.navbar.classList.remove(this.animationSliderCssClass);
                this.hideNavbar();
                this.isNavbarOpened = false;
                this.isSmallScreen = false;
            } else {
                this.setSmallScreenCss();
            }
        });
    }

    private addNavbarClickEvent() {
        this.navbar.addEventListener("click", () => {
            if (this.isSmallScreen) {
                this.isNavbarOpened = false;
                this.hideNavbar();
            }
        });
    }

    private addToggleButtonEvent() {
        var toggleButton = this.nativeElement.querySelector("button[class='navbar-toggle']");
        toggleButton.addEventListener("click", () => {
            this.isNavbarOpened = !this.isNavbarOpened;
            if (this.isNavbarOpened) {
                this.showNavbar();
            } else {
                this.hideNavbar();
            }
        });
    }

    private hideNavbar() {
        this.navbar.classList.add(this.closeCssClass);
    }

    private showNavbar() {
        this.navbar.classList.remove(this.closeCssClass);
    }
}