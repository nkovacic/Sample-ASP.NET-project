import { CacheKey, Deserializer, Serializer } from 'json-object-mapper';
import * as format from 'date-fns/format'
import * as parse from 'date-fns/parse'

export class DateSerializer implements Deserializer, Serializer {
    public deserialize = (value: string) => {
        if (value) {
            return parse(value, null, null);
        }
        else {
            return Date.now();
        }
    }

    public serialize = (dateTime: Date) => {
        return format(dateTime, null);
    }
}
