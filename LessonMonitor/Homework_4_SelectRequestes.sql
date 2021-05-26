use Homework_4

SELECT *FROM Students 
WHERE TypeAccount IN ('Watcher', 'Admin')

SELECT DISTINCT TOPIC FROM Questions

SELECT Nickname FROM Students
SELECT Name From Lessons

--���-�� �������� � ���������
SELECT COUNT (*) AS CountQuestion FROM Questions
SELECT COUNT (*) AS CountTimecodes FROM Timecodes

--���-�� ��������� ������
SELECT LessonID, COUNT (*) AS CountVisit FROM VisitorsLessons
GROUP BY LessonID

--����� �� ����
SELECT *FROM Lessons ORDER BY Date DESC

--�������� �� �����
SELECT Timecodes.Name, Timecodes.Timecode, Lessons.Name AS LessonName
FROM Timecodes
INNER JOIN Lessons ON Lessons.ID = Timecodes.LessonId
WHERE Lessons.ID = 2
