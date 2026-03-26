import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';

@Component({   // This is the root component of the Angular application. It serves as the main container for the app and defines the overall structure of the UI. The RouterOutlet directive is used to render the components associated with the current route, allowing for navigation between different views in the application.
  selector: 'app-root',   // The selector 'app-root' is the custom HTML tag that will be used to insert this component into the index.html file. This is the entry point of the Angular application, and the content of this component will be rendered inside the <app-root> tag in the HTML.
  standalone: true,   // standalone means this component can be used without being declared in an NgModule. This is a new feature in Angular that allows for more modular and flexible component definitions.
  imports: [RouterOutlet],   // This component imports the RouterOutlet directive, which is necessary for rendering the routed components. The RouterOutlet acts as a placeholder in the template where the matched component for the current route will be displayed.
  template: `<router-outlet />`   // The template for this component consists of a single <router-outlet> element. This is where the Angular router will render the component that corresponds to the current route. When the user navigates to different routes in the application, the content inside the <router-outlet> will change accordingly, allowing for a dynamic and responsive user interface.
})
export class AppComponent {     // The AppComponent class is the main component class for the application. It currently only has a title property, which is set to 'saas invoice system'. This title can be used in the template or elsewhere in the application to display the name of the app or for other purposes. The AppComponent serves as the root component that bootstraps the entire Angular application, and it can contain global logic or state that needs to be shared across the app.
  title = 'Invoice System';
}
