# cbs-sports-api
Play with data from CBS Sports API For Player

# Running
To run locally, simply run `dotnet run`. As the database is in-memory, there are no external requirements.

# Testing

## Importing Data
Data can be imported explicitly via this sync API:

```shell
curl --location --request POST 'localhost:5135/players/sync'
```

which simply returns a `204 No Content`.
When data needs to be updated, it can be run again.

## Retrieving A Player
To retrieve a player by Id, use the API `/players/:sport/:id` like so:

```shell
curl --location 'localhost:5135/players/football/3124706'
```

As ids are only unique within a sport, the sport is part of the path.  The response will look like the following:

```json
{
    "id": 3124706,
    "first_name": "Adetomiwa",
    "last_name": "Adebawore",
    "position": "DL",
    "age": 22,
    "average_position_age_diff": 4.130982367758186,
    "name_brief": "A. Adebawore"
}
```

## Searching For A Player
To search for a player use this API:

```shell
curl --location 'localhost:5135/players/search' \
--header 'Content-Type: application/json' \
--data '{
    "sport": "basketball",
    "position": "PF",
    "minAge": 20,
    "maxAge": 30,
    "firstLetterLastName": "L"
}'
```

Each of the parameters in the body is optional. If a specific age is desired (eg 22), then set the min and max age to that value. The response is identical to the retrieving player API:

```json
{
    "id": 2152530,
    "first_name": "Trey",
    "last_name": "Lyles",
    "position": "PF",
    "age": 27,
    "average_position_age_diff": 1.5428571428571445,
    "name_brief": "Trey L."
}
```

# Overall Strategy
The overall strategy in this API is to be agile and only build out as much as is needed. Without additional information about future enhancements, we also cannot make predictions about where this code will go, such as:
- Will the different sports eventually be separated as features diverge?
- How/when will the import/resync need to occur? If it is frequent, it could move to a cron job/scheduled task, or if it is less frequent it can be an occasional developer chore.
- How many additional features will be added to this application? There is still some logic in the controllers that could be extracted to services to make testing easier.

# Caveats/Cut Corners

## In-Memory Database
This was done due to time constraints on the project. With additional time, this can connect to a real database either locally (through docker/natively) or remotely (to a dev database).
Additionally, with a formal database, the schema would be controlled via database migrations as opposed to manual sql scripts. In-memory databases do not support schema migrations and thus none are currently present.

## Unique Constraint On Player
Currently, all of the player data is stored in a single table. This is because there is no additional information for how this data will be used/extended. Unfortunately, this causes a problem with some players such as `Austin Aune`, who played multiple sports, with a single Id for each. Because of this, the data is considered unique across both sport and id. There are other (more complex) ways this data could have been structured:
1. Creating a secondary table for sport-specific data that would contain {sport, position}. This means that Austin would get two rows in this lookup table and one in the general Player table.
2. Creating separate tables for each sport to segregate the data. This would be beneficial if the sports began to have different feature sets.

## Import Optimization
For now, the import is a straightforward wipe all and re-import. This is good for a first pass, but can cause potential issues down the line:
- Performance issues with mass deletes and/or database table fragmentation
- Data inconsistencies if some data was modified in this data store compared to the data retrieved from the Cbs Api

With additional information, this import could be optimized in a few ways:
1. Convert to an "upsert"/merge approach. This will insert when not found or update when different. The update would depend on which data we want to be the "source of truth".
2. Assuming the API provider allows it, we could subscribe to a message queue from the API provider for only new changes, or we could potentially get this data from a time-diff API (provide the list of players with a modified time within the last 7 days).
