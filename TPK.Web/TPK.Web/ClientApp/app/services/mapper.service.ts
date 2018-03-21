export class Mapper {
    static map(source: any, destination: any) {
        if (source) {
            for (var prop in source) {
                if (source.hasOwnProperty(prop))
                    destination[prop] = source[prop];
            }
        }
    }
}