-- update checkins
	
UPDATE businessTable
    SET numcheckins = temp.sum
FROM (SELECT temp.business_id, SUM(temp.sum) 
      FROM (SELECT *, (morning + afternoon + evening + night) AS Sum
            FROM checkintable) as temp
      GROUP BY temp.business_id) as temp
WHERE businessTable.business_id = temp.business_id

-- update reviewratings

UPDATE businessTable
	SET reviewrating = temp.averagerating
FROM (SELECT temp.business_id, (temp.sum / temp.count) as averageRating
	  FROM (SELECT business_id, SUM(stars), COUNT(business_id)
	        FROM reviewTable
      GROUP BY business_id) as temp) as temp
WHERE businessTable.business_id = temp.business_id

-- update reviewcount

UPDATE businessTable
SET review_count = temp.rCount
FROM (SELECT temp.business_id, (temp.review_count + 1) as rCount
     FROM (SELECT business_id, review_count
          FROM businessTable
          GROUP BY business_id) as temp ) as temp
     WHERE businessTable.business_id = temp.business_id