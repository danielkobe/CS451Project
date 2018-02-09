import json

def cleanStr4SQL(s):
    return s.replace("'","`").replace("\n"," ")

def parseBusinessData():
    #read the JSON file
    with open('.\yelp_business.JSON','r') as f:  #Assumes that the data files are available in the current directory. If not, you should set the path for the yelp data files.
        outfile =  open('business.txt', 'w')
        outfile.write('business_id,  name,  address,  state,  city,  postal_code,  latitude,  longitude,  stars,  review_count,  is_open,  [categories],  [attributes],  [hours]\n')
        line = f.readline()
        count_line = 0
        #read each JSON abject and extract data
        while line:
            data = json.loads(line)
            outfile.write(cleanStr4SQL(data['business_id']) + ', ') #business id
            outfile.write(cleanStr4SQL(data['name']) + ', ') #name
            outfile.write(cleanStr4SQL(data['address']) + ', ') #full_address
            outfile.write(cleanStr4SQL(data['state']) + ', ') #state
            outfile.write(cleanStr4SQL(data['city']) + ', ') #city
            outfile.write(cleanStr4SQL(data['postal_code']) + ', ')  #zipcode
            outfile.write(str(data['latitude']) + ', ') #latitude
            outfile.write(str(data['longitude']) + ', ') #longitude
            outfile.write(str(data['stars']) + ', ') #stars
            outfile.write(str(data['review_count']) + ', ') #reviewcount
            outfile.write(str(data['is_open']) + '\n') #openstatus

            #process attributes (sample code included quote marks in parsed text)
            outfile.write('Categories:  [')
            for item in data['categories']:
                outfile.write(item + ', ')
            outfile.write(']\n')

            #process attributes
            attributes = data['attributes']
            outfile.write('Attributes: [')
            for attr, val in attributes.items():
                # non-nested attribute
                if type(val) == str or type(val) == bool:
                    outfile.write('(' + str(attr) + ', ' + str(val) + ')')
                # nested attributes
                if type(val) == dict:
                    for nested_attr, nested_val in val.items():
                        outfile.write('(' + str(nested_attr) + ', ' + str(nested_val) + ')')
            outfile.write(']\n')

            #process hours
            hours = data['hours']
            outfile.write('Hours: [')
            for day, hours in hours.items():
                split_hours = hours.split('-')
                outfile.write('(' + day + ', ' + split_hours[0] + ',' + split_hours[1] + ')')
            outfile.write(']\n');

            line = f.readline()
            count_line +=1
    print(count_line)
    outfile.close()
    f.close()

def parseUserData():

    with open('.\yelp_user.JSON', 'r') as f:  # Assumes that the data files are available in the current directory. If not, you should set the path for the yelp data files.
        outfile =  open('user.txt', 'w')
        outfile.write('user_id,  name,  yelping_since,  review_count,  fans,  average_stars,  funny,  useful,  cool,  friends\n')
        line = f.readline()
        count_line = 0
        #read each JSON abject and extract data
        while line:
            data = json.loads(line)
            outfile.write(cleanStr4SQL(data['user_id']) + ', ') #user_id
            outfile.write(cleanStr4SQL(data['name']) + ', ') #name
            outfile.write(cleanStr4SQL(data['yelping_since']) + ', ') #yelping_since
            outfile.write(str(data['review_count']) + ', ') #review_count
            outfile.write(str(data['fans']) + ', ') #fans
            outfile.write(str(data['average_stars']) + ', ')  #average_stars
            outfile.write(str(data['funny']) + ', ') #funny
            outfile.write(str(data['useful']) + ', ') #useful
            outfile.write(str(data['cool']) + '\n') #cool
            outfile.write('Friends: [')
            for value in data['friends']:
                outfile.write(value + ' , ')
            outfile.write(']\n')
            line = f.readline()
            count_line +=1
    print(count_line)
    outfile.close()
    f.close()

def parseCheckinData():
    with open('.\yelp_checkin.JSON', 'r') as f:
        outfile = open('checkin.txt', 'w')
        outfile.write('business_id,  monday_visits, mon_morning, mon_afternoon, mon_evening, mon_night, tuesday_visits, tue_morning, tue_afternoon, tue_evening, tue_night, ')
        outfile.write('wednesday_visits, wed_morning, wed_afternoon, wed_evening, wed_night, thursday_visits, thu_morning, thu_afternoon, thu_evening, thu_night, ')
        outfile.write('friday_visits, fri_morning, fri_afternoon, fri_evening, fri_night, saturday_visits, sat_morning, sat_afternoon, sat_evening, sat_night, ')
        outfile.write('sunday_visits, sun_morning, sun_afternoon, sun_evening, sun_night\n')
        line = f.readline()
        count_line = 0
        visits_total = {'Monday': 0, 'Tuesday': 0, 'Wednesday': 0, 'Thursday': 0, 'Friday': 0, 'Saturday': 0, 'Sunday': 0}
        visits = {'Morning': 0, 'Afternoon': 0, 'Evening': 0, 'Night': 0}
        visitsList = {'Monday': {}, 'Tuesday': {}, 'Wednesday': {}, 'Thursday': {}, 'Friday': {}, 'Saturday': {}, 'Sunday': {}}
        data = json.loads(line)
        while line:
            data = json.loads(line)
            outfile.write(data['business_id'] + ', ')
            for day in data['time']:
                for hour in data['time'][day]:
                    hr = int(hour.split(':')[0])
                    if (hr >= 6 and hr < 12):
                        visits['Morning'] += data['time'][day][hour]
                    elif (hr >= 12 and hr < 17):
                        visits['Afternoon'] += data['time'][day][hour]
                    elif (hr >= 17 and hr < 23):
                        visits['Evening'] += data['time'][day][hour]
                    else:
                        visits['Night'] += data['time'][day][hour]
                for item in visits:
                    visits_total[day] += visits[item]
                visitsList[day] = visits
                visits = dict.fromkeys(visits, 0)
            for day in visits_total:
                outfile.write(str(visits_total[day]) + ', ')
                for item in visitsList[day]:
                    outfile.write(str(visitsList[day][item]) + ', ')
            outfile.write('\n')
            visits_total = dict.fromkeys(visits_total, 0)
            visits = dict.fromkeys(visits, 0)
            line = f.readline()
            count_line += 1
        print(count_line)
        outfile.close()
        f.close()


def parseReviewData():
    with open('.\yelp_review.JSON', 'r') as f:  # Assumes that the data files are available in the current directory. If not, you should set the path for the yelp data files.
        outfile =  open('review.txt', 'w')
        outfile.write('review_id,  user_id,  business_id,  text,  stars,  date,  funny,  useful,  cool\n')
        line = f.readline()
        count_line = 0
        #read each JSON abject and extract data
        while line:
            data = json.loads(line)
            outfile.write(cleanStr4SQL(data['review_id']) + ', ') #review_id
            outfile.write(cleanStr4SQL(data['user_id']) + ', ') #user_id
            outfile.write(cleanStr4SQL(data['business_id']) + ', ') #business_id
            outfile.write(cleanStr4SQL(data['text']) + ', ') #text
            outfile.write(str(data['stars']) + ', ') #stars
            outfile.write(cleanStr4SQL(data['date']) + ', ')  #date
            outfile.write(str(data['funny']) + ', ') #funny
            outfile.write(str(data['useful']) + ', ') #useful
            outfile.write(str(data['cool']) + '\n') #cool

            line = f.readline()
            count_line +=1
    print(count_line)
    outfile.close()
    f.close()

parseBusinessData()
parseUserData()
parseCheckinData()
parseReviewData()
