<script>
    async function SendData(message, messageHistory){
        const response = await fetch("http://localhost:5156/message", {
            method: "POST",

            body: JSON.stringify({Message:message, Messages:messageHistory})
        })
    }

    let message = ""
    let messageHistory = ["tdrtdhellodddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddd", "asd"]
    let align=""
</script>

<div class="flex flex-col overflow-y-auto w-1/2 border-2 p-10 rounded-2xl">
    {#each messageHistory as message, i}
        

		<span class={`${i % 2 ? "bg-red-500" : "bg-blue-500 self-end"} w-auto px-2 py-4 rounded-md max-w-1/3 wrap-anywhere relative after:block after:absolute after:right-0 after:bottom-0 after:w-2 ${i % 2 ? "after:bg-red-500 after:left-0" : "after:bg-blue-500"} after:aspect-square after:`}>{message}</span>
	{/each}
</div>