const fetchGames = (page, perPage = 10) => {
    // 注意：此处我们仍然通过查询字符串发送分页参数
    const url = `/home/hist_show?page=${page}&per_page=${perPage}`;
    fetch(url, {
        method: 'POST', // 设置请求方法为POST
        headers: {
            // Flask后端可能不需要这个请求头，因为我们不发送JSON数据
            'Accept': 'application/json', // 明确期望得到JSON格式的响应
        },
    })
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(data => {
            const gamesListEl = document.getElementById('games-list');
            gamesListEl.innerHTML = ''; // 清空当前内容
            data.forEach(game => {
                const gameEl = document.createElement('div');
                gameEl.className = 'game-info';
                // 截取时间
                const createTime = game.create_at.slice(0, 16); // "YYYY-MM-DDTHH:MM"
                const playTime = game.play_datetime ? game.play_datetime.slice(0, 16) : '未知';

                gameEl.innerHTML = `
                <div><strong>对局名称：</strong>${game.game_name}</div>
                <div><strong>创建时间：</strong>${createTime}</div>
                <div><strong>对局时间：</strong>${playTime}</div>
                <div><strong>创建者：</strong>${game.user_id}</div>
            `;
                gamesListEl.appendChild(gameEl);
            });
        })
        .catch(error => {
            console.error('Error fetching data: ', error);
            // 可以在这里处理错误，例如显示错误信息
        });
};

document.addEventListener('DOMContentLoaded', function () {
    const prevPageButton = document.getElementById('prev-page');
    const nextPageButton = document.getElementById('next-page');
    const currentPageEl = document.getElementById('current-page');
    let currentPage = 1; // 初始页码为1

    // 上一页按钮点击事件
    prevPageButton.addEventListener('click', () => {
        if (currentPage > 1) {
            currentPage -= 1;
            fetchGames(currentPage);
            currentPageEl.innerText = currentPage;
        }
    });

    // 下一页按钮点击事件
    nextPageButton.addEventListener('click', () => {
        currentPage += 1; // 增加页码
        fetchGames(currentPage);
        currentPageEl.innerText = currentPage;
    });

    // 初始加载第一页的历史对局数据
    fetchGames(currentPage);
});
