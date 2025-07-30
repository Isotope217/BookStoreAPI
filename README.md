# AutoFlow Tech Test Instructions
You are invited to complete the AutoFlow Tech Test as part of the next stage in our interview process. We ask that you submit your solution prior to your upcoming interview, where we’ll review your work and discuss the decisions you made during the exercise.
We’ve provided a starter solution to help you get going, but feel free to modify it as much as you like to best demonstrate your skills and strengths.
You should aim to spend no more than 1–2 hours on this task. You’re welcome to use AI tools to assist you, but please note that we’ll ask questions during the review to assess your understanding of the code and the approach you’ve taken.

## The Exercise
The AutoFlow Bookstore allows users to view a list of all available books, information about each title, and it’s rating. It also includes functionality to search for books and add new entries to the collection.
In this exercise, we’d like you to implement a feature that filters the list of books by rating. To do this effectively, you may need to refactor parts of the existing code base, as some poor practices have crept in over time.
Feel free to modify any part of the solution to best showcase your skills and your approach to clean, maintainable code.
To implement the feature successfully, the following acceptance criteria must be met:

**Scenario: Display all books by default**  
  Given I am on the bookstore page  
  When I have not selected any rating filter  
  Then I should see the full list of books  

**Scenario: Filter books by a specific rating**  
  Given I am on the bookstore page  
  When I select a rating value from the rating filter  
  Then I should see only the books with the selected rating  

**Scenario: No books match the selected rating**  
  Given I am on the bookstore page  
  When I select a rating that no books have  
  Then I should see a message indicating no results were found  

**Scenario: Clear the rating filter**  
  Given I have selected a rating filter  
  When I clear or reset the filter  
  Then I should see the full list of books again  
